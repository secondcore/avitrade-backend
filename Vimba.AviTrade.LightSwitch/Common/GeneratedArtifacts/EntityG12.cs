﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LightSwitchApplication
{
    #region Entities
    
    /// <summary>
    /// No Modeled Description Available
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
    public sealed partial class TraderRegistrationToken : global::Microsoft.LightSwitch.Framework.Base.EntityObject<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass>
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new instance of the TraderRegistrationToken entity.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public TraderRegistrationToken()
            : this(null)
        {
        }
    
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public TraderRegistrationToken(global::Microsoft.LightSwitch.Framework.EntitySet<global::LightSwitchApplication.TraderRegistrationToken> entitySet)
            : base(entitySet)
        {
            global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.Initialize(this);
        }
    
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void TraderRegistrationToken_Created();
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void TraderRegistrationToken_AllowSaveWithErrors(ref bool result);
    
        #endregion
    
        #region Private Properties
        
        /// <summary>
        /// Gets the Application object for this application.  The Application object provides access to active screens, methods to open screens and access to the current user.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private global::Microsoft.LightSwitch.IApplication<global::LightSwitchApplication.DataWorkspace> Application
        {
            get
            {
                return global::LightSwitchApplication.Application.Current;
            }
        }
        
        /// <summary>
        /// Gets the containing data workspace.  The data workspace provides access to all data sources in the application.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private global::LightSwitchApplication.DataWorkspace DataWorkspace
        {
            get
            {
                return (global::LightSwitchApplication.DataWorkspace)this.Details.EntitySet.Details.DataService.Details.DataWorkspace;
            }
        }
        
        #endregion
    
        #region Public Properties
    
        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public int Id
        {
            get
            {
                return global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.GetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Id);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Id_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Id_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Id_Changed();

        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime CreateDate
        {
            get
            {
                return global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.GetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.CreateDate);
            }
            set
            {
                global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.SetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.CreateDate, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void CreateDate_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void CreateDate_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void CreateDate_Changed();

        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string Token
        {
            get
            {
                return global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.GetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Token);
            }
            set
            {
                global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.SetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Token, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Token_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Token_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Token_Changed();

        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::LightSwitchApplication.Trader Trader
        {
            get
            {
                return global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.GetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Trader);
            }
            set
            {
                global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.SetValue(this, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Trader, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Trader_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Trader_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Trader_Changed();

        #endregion
    
        #region Details Class
    
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public sealed class DetailsClass : global::Microsoft.LightSwitch.Details.Framework.Base.EntityDetails<
                global::LightSwitchApplication.TraderRegistrationToken,
                global::LightSwitchApplication.TraderRegistrationToken.DetailsClass,
                global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.IImplementation,
                global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySet,
                global::Microsoft.LightSwitch.Details.Framework.EntityCommandSet<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass>,
                global::Microsoft.LightSwitch.Details.Framework.EntityMethodSet<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass>>
        {
    
            static DetailsClass()
            {
                var initializeEntry = global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Id;
            }
    
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private static readonly global::Microsoft.LightSwitch.Details.Framework.Base.EntityDetails<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass>.Entry
                __TraderRegistrationTokenEntry = new global::Microsoft.LightSwitch.Details.Framework.Base.EntityDetails<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass>.Entry(
                    global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.__TraderRegistrationToken_CreateNew,
                    global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.__TraderRegistrationToken_Created,
                    global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.__TraderRegistrationToken_AllowSaveWithErrors);
            private static global::LightSwitchApplication.TraderRegistrationToken __TraderRegistrationToken_CreateNew(global::Microsoft.LightSwitch.Framework.EntitySet<global::LightSwitchApplication.TraderRegistrationToken> es)
            {
                return new global::LightSwitchApplication.TraderRegistrationToken(es);
            }
            private static void __TraderRegistrationToken_Created(global::LightSwitchApplication.TraderRegistrationToken e)
            {
                e.TraderRegistrationToken_Created();
            }
            private static bool __TraderRegistrationToken_AllowSaveWithErrors(global::LightSwitchApplication.TraderRegistrationToken e)
            {
                bool result = false;
                e.TraderRegistrationToken_AllowSaveWithErrors(ref result);
                return result;
            }
    
            public DetailsClass() : base()
            {
            }
    
            public new global::Microsoft.LightSwitch.Details.Framework.EntityCommandSet<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass> Commands
            {
                get
                {
                    return base.Commands;
                }
            }
    
            public new global::Microsoft.LightSwitch.Details.Framework.EntityMethodSet<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass> Methods
            {
                get
                {
                    return base.Methods;
                }
            }
    
            public new global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySet Properties
            {
                get
                {
                    return base.Properties;
                }
            }
    
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed class PropertySet : global::Microsoft.LightSwitch.Details.Framework.Base.EntityPropertySet<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass>
            {
    
                public PropertySet() : base()
                {
                }
    
                public global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, int> Id
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Id) as global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, int>;
                    }
                }
                
                public global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::System.DateTime> CreateDate
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.CreateDate) as global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::System.DateTime>;
                    }
                }
                
                public global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, string> Token
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Token) as global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, string>;
                    }
                }
                
                public global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::LightSwitchApplication.Trader> Trader
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Trader) as global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::LightSwitchApplication.Trader>;
                    }
                }
                
            }
    
            #pragma warning disable 109
            [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public interface IImplementation : global::Microsoft.LightSwitch.Internal.IEntityImplementation
            {
                new int Id { get; }
                new global::System.DateTime CreateDate { get; set; }
                new string Token { get; set; }
                new global::Microsoft.LightSwitch.Internal.IEntityImplementation Trader { get; set; }
            }
            #pragma warning restore 109
    
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "11.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal class PropertySetProperties
            {
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, int>.Entry
                    Id = new global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, int>.Entry(
                        "Id",
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Id_Stub,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Id_ComputeIsReadOnly,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Id_Validate,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Id_GetImplementationValue,
                        null,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Id_OnValueChanged);
                private static void _Id_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, int>.Data> c, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, object sf)
                {
                    c(d, ref d._Id, sf);
                }
                private static bool _Id_ComputeIsReadOnly(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    bool result = false;
                    e.Id_IsReadOnly(ref result);
                    return result;
                }
                private static void _Id_Validate(global::LightSwitchApplication.TraderRegistrationToken e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.Id_Validate(r);
                }
                private static int _Id_GetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d)
                {
                    return d.ImplementationEntity.Id;
                }
                private static void _Id_OnValueChanged(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    e.Id_Changed();
                }
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::System.DateTime>.Entry
                    CreateDate = new global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::System.DateTime>.Entry(
                        "CreateDate",
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._CreateDate_Stub,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._CreateDate_ComputeIsReadOnly,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._CreateDate_Validate,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._CreateDate_GetImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._CreateDate_SetImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._CreateDate_OnValueChanged);
                private static void _CreateDate_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::System.DateTime>.Data> c, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, object sf)
                {
                    c(d, ref d._CreateDate, sf);
                }
                private static bool _CreateDate_ComputeIsReadOnly(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    bool result = false;
                    e.CreateDate_IsReadOnly(ref result);
                    return result;
                }
                private static void _CreateDate_Validate(global::LightSwitchApplication.TraderRegistrationToken e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.CreateDate_Validate(r);
                }
                private static global::System.DateTime _CreateDate_GetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d)
                {
                    return d.ImplementationEntity.CreateDate;
                }
                private static void _CreateDate_SetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, global::System.DateTime v)
                {
                    d.ImplementationEntity.CreateDate = global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.ClearDateTimeKind(v);
                }
                private static void _CreateDate_OnValueChanged(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    e.CreateDate_Changed();
                }
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, string>.Entry
                    Token = new global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, string>.Entry(
                        "Token",
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Token_Stub,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Token_ComputeIsReadOnly,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Token_Validate,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Token_GetImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Token_SetImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Token_OnValueChanged);
                private static void _Token_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, string>.Data> c, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, object sf)
                {
                    c(d, ref d._Token, sf);
                }
                private static bool _Token_ComputeIsReadOnly(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    bool result = false;
                    e.Token_IsReadOnly(ref result);
                    return result;
                }
                private static void _Token_Validate(global::LightSwitchApplication.TraderRegistrationToken e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.Token_Validate(r);
                }
                private static string _Token_GetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d)
                {
                    return d.ImplementationEntity.Token;
                }
                private static void _Token_SetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, string v)
                {
                    d.ImplementationEntity.Token = v;
                }
                private static void _Token_OnValueChanged(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    e.Token_Changed();
                }
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::LightSwitchApplication.Trader>.Entry
                    Trader = new global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::LightSwitchApplication.Trader>.Entry(
                        "Trader",
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_Stub,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_ComputeIsReadOnly,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_Validate,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_GetCoreImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_GetImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_SetImplementationValue,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_Refresh,
                        global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties._Trader_OnValueChanged);
                private static void _Trader_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::LightSwitchApplication.Trader>.Data> c, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, object sf)
                {
                    c(d, ref d._Trader, sf);
                }
                private static bool _Trader_ComputeIsReadOnly(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    bool result = false;
                    e.Trader_IsReadOnly(ref result);
                    return result;
                }
                private static void _Trader_Validate(global::LightSwitchApplication.TraderRegistrationToken e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.Trader_Validate(r);
                }
                private static global::Microsoft.LightSwitch.Internal.IEntityImplementation _Trader_GetCoreImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d)
                {
                    return d.ImplementationEntity.Trader;
                }
                private static global::LightSwitchApplication.Trader _Trader_GetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d)
                {
                    return d.GetImplementationValue<global::LightSwitchApplication.Trader, global::LightSwitchApplication.Trader.DetailsClass>(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Trader, ref d._Trader);
                }
                private static void _Trader_SetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d, global::LightSwitchApplication.Trader v)
                {
                    d.SetImplementationValue(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Trader, ref d._Trader, (i, ev) => i.Trader = ev, v);
                }
                private static void _Trader_Refresh(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass d)
                {
                    d.RefreshNavigationProperty(global::LightSwitchApplication.TraderRegistrationToken.DetailsClass.PropertySetProperties.Trader, ref d._Trader);
                }
                private static void _Trader_OnValueChanged(global::LightSwitchApplication.TraderRegistrationToken e)
                {
                    e.Trader_Changed();
                }
    
            }
    
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, int>.Data _Id;
            
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::System.DateTime>.Data _CreateDate;
            
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, string>.Data _Token;
            
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.TraderRegistrationToken, global::LightSwitchApplication.TraderRegistrationToken.DetailsClass, global::LightSwitchApplication.Trader>.Data _Trader;
            
        }
    
        #endregion
    }
    
    #endregion
}
