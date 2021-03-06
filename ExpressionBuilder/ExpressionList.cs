﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;

namespace ChabadOnCampus.DynamicLinq
{
    public class ExpressionVM: INotifyPropertyChanged
    {

        IEnumerable<PropertyInfo> _AvailableProperties;
        public IEnumerable<PropertyInfo> AvailableProperties
        {
            get { return _AvailableProperties; }
            set
            {
                _AvailableProperties = value;
                OnPropertyChanged("AvailableProperties");
            }
        }

        public Array AvailableCompareOperators
        {
            get 
            {
                if (PropertyType == typeof(string))
                {
                    return new ComparisonOperators[] { ComparisonOperators.Equal, ComparisonOperators.NotEqual, ComparisonOperators.Contains };
                }
                else
                {
                    var operators = from o in Enum.GetValues(typeof(ComparisonOperators)).Cast<ComparisonOperators>()
                                    where o != ComparisonOperators.Contains // exclude "Contains" as only works for String
                                    select o;
                    return operators.ToArray();
                }
            }
        }
        public Array AvailableCombinationOperators
        {
            get { return Enum.GetValues(typeof(CombinationOperators)); }
        }

        private Type _ObjectType;
	    public Type ObjectType
	    {
		    get { return _ObjectType;}
            set { 
                _ObjectType = value;
                if (value == null)
                {
                    AvailableProperties = null;
                }
                else
                {
                    AvailableProperties = from p in value.GetProperties()
                                          where GetSupportedTypes().Contains(p.PropertyType) &&
                                          p.GetCustomAttributes(typeof(DoNotFilterOnAttribute), true).Length == 0 // in other words, where attribute is NOT applied
                                          select p;
                }
                OnPropertyChanged("ObjectType");
            }
	    }
	
        private ComparisonOperators _CompareOperator = ComparisonOperators.Equal;
        public ComparisonOperators CompareOperator
        {
            get { return _CompareOperator; }
            set { _CompareOperator = value; OnPropertyChanged("CompareOperator"); }
        }

        private CombinationOperators _CombineOperator = CombinationOperators.And;
        public CombinationOperators CombineOperator
        {
            get { return _CombineOperator; }
            set { _CombineOperator = value; OnPropertyChanged("CombineOperator"); }
        }

        private PropertyInfo _PropertyInfo;
        public PropertyInfo PropertyInfo
        {
            get { return _PropertyInfo; }
            set {
                _PropertyInfo = value;
                OnPropertyChanged("PropertyInfo");
                OnPropertyChanged("PropertyType");
                OnPropertyChanged("PropertyName");
                OnPropertyChanged("AvailableCompareOperators");
                OnPropertyChanged("Value2");
                OnPropertyChanged("ValueControl");
            }
        }
 	    public Type PropertyType
	    {
            get { return PropertyInfo == null? typeof(string) : PropertyInfo.PropertyType; }
	    }
        public string PropertyName
        {
            get { return PropertyInfo == null ? null : PropertyInfo.Name; }
        }

        public dynamic Value
        {
            get { return Value2.Value; }
            set
            {
                if (!Object.Equals(Value2.Value, value))
                {
                    Value2.Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
        private object _Value2;
        public dynamic Value2
        {
            get {
                Type specificType = typeof(Variant<>).MakeGenericType(new Type[] { PropertyType });
                if(_Value2 == null || _Value2.GetType() != specificType)
                {
                    _Value2 = Activator.CreateInstance(specificType);
                }
                return _Value2;
            }
        }

        public Control ValueControl 
        {
            get
            {
                Binding binding = new Binding("Value2.Value");
                binding.Source = this;

                if (PropertyType == typeof(DateTime) || PropertyType == typeof(DateTime?))
                {
                    DatePicker dp = new DatePicker();
                    dp.SetBinding(DatePicker.SelectedDateProperty, binding);
                    return dp;
                }
                else
                {
                    TextBox tb = new TextBox();
                    tb.SetBinding(TextBox.TextProperty, binding);
                    return tb;
                }
            }
        }

        public static object CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            System.Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }
        public LambdaExpression MakeExpression(ParameterExpression paramExpr = null)
        {
            if(paramExpr == null) paramExpr = Expression.Parameter(ObjectType, "left");
            
            var callExpr = Expression.MakeMemberAccess(paramExpr, PropertyInfo);
            var valueExpr = Expression.Constant(Value, PropertyType);
            Expression expr;

            if (CompareOperator == ComparisonOperators.Contains)
            {
                expr = Expression.Call(callExpr, PropertyType.GetMethod("Contains"), valueExpr);
            }
            else
            {
                expr = Expression.MakeBinary((ExpressionType)CompareOperator, callExpr, valueExpr);
            }
            
            return Expression.Lambda( expr, paramExpr);
        }

        public static IEnumerable<Type> GetSupportedTypes()
        {
            return new Type[] {typeof(DateTime), typeof(DateTime?),
                                          typeof(long), typeof(long?), typeof(short), typeof(short?), 
                                          typeof(ulong), typeof(ulong?), typeof(ushort), typeof(ushort?), 
                                          typeof(Single), typeof(Single?), typeof(Double), typeof(Double?), 
                                          typeof(Decimal), typeof(Decimal?), typeof(Boolean), typeof(Boolean?), 
                                          typeof(int), typeof(int?), typeof(uint), typeof(uint), 
                                          typeof(string)};
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string value)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(value));
        }
    }

    public class ExpressionList: ObservableCollection<ExpressionVM>
    {

        private Type _Type;
        public Type Type
        {
            get { return _Type; }
            set {
                _Type = value;
                
                foreach(ExpressionVM c in this)
                {
                    //c.AvailableProperties = props.ToArray().Clone() as PropertyInfo[];
                    c.ObjectType = value;
                }
            }
        }

        protected override void InsertItem(int index, ExpressionVM item)
        {
            base.InsertItem(index, item);
            if (item.ObjectType == null) item.ObjectType = Type;
        }

        public LambdaExpression CompleteExpression
        {
            get
            {
                var paramExp = Expression.Parameter(Type, "left");
                if (this.Count == 0) return Expression.Lambda(Expression.Constant(true), paramExp);
                LambdaExpression lambda1 = this.First().MakeExpression(paramExp);
                var ret = lambda1.Body;
                foreach (var c in this.Skip(1))
                    ret = Expression.MakeBinary((ExpressionType)c.CombineOperator, ret, c.MakeExpression(paramExp).Body);
                return Expression.Lambda(ret, paramExp);
            }
        }

        public Expression<Func<T, bool>> GetCompleteExpression<T>()
        {
            return this.CompleteExpression as Expression<Func<T, bool>>;
        }
    }

    public enum ComparisonOperators
    {
            Equal = ExpressionType.Equal,
            NotEqual = ExpressionType.NotEqual,
            LessThan = ExpressionType.LessThan,
            GreaterThan = ExpressionType.GreaterThan,
            LessThanOrEqual = ExpressionType.LessThanOrEqual,
            GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
            Contains
    }

    public enum CombinationOperators
    {
            Or = ExpressionType.Or,
            And = ExpressionType.And,
            Xor = ExpressionType.ExclusiveOr,
    }

    public struct Variant<T>
    {
        public T Value { get; set; }
    }
}
