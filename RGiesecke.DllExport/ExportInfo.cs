/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace RGiesecke.DllExport
{
    using Stfld = KeyValuePair<string, object>;
    using Stflds = Dictionary<string, object>;

    [Serializable]
    public class ExportInfo: IExportInfo
    {
        private Stflds metaValues;

        public virtual string ExportName
        {
            get;
            set;
        }

        public CallingConvention CallingConvention
        {
            get;
            set;
        }

        public bool IsStatic
        {
            get;
            set;
        }

        public bool IsGeneric
        {
            get;
            set;
        }

        public void AssignFrom(IExportInfo info, IInputValues input)
        {
            if(info == null || String.IsNullOrWhiteSpace(input.MetaLib)) {
                throw new ArgumentNullException("IExportInfo or path to MetaLib cannot be null.");
            }

            setDefaultValues(input);
            setUserValues(info);
        }

        protected void setUserValues(IExportInfo info)
        {
            if(info.CallingConvention != 0) {
                CallingConvention = info.CallingConvention;
            }

            if(info.ExportName != null) {
                ExportName = info.ExportName;
            }
        }

        protected void setDefaultValues(IInputValues input)
        {
            if(metaValues == null) {
                metaValues = getMetaValues(getMetaCtor(input));
            }

            Func<string, object, object> _ = delegate(string key, object def) {
                return metaValues.ContainsKey(key) ? metaValues[key] : def;
            };

            CallingConvention   = (CallingConvention) _("CallingConvention", CallingConvention.Cdecl);
            ExportName          = (string)_("ExportName", null);
        }

        private Stflds getMetaValues(MethodDefinition md)
        {
            var ret = new Stflds();
            foreach(var inst in md.Body.Instructions) {
                if(inst.OpCode.Code != Code.Stfld) {
                    continue;
                }

                var cv = ctorValues(inst);
                if(cv.Key != null) {
                    ret[cv.Key] = cv.Value;
                }
            }
            return ret;
        }

        private MethodDefinition getMetaCtor(IInputValues input)
        {
            ModuleDefinition module = ModuleDefinition.ReadModule(input.MetaLib);

            var _class = module.GetTypes()
                                .Where(t => t.FullName == input.DllExportAttributeFullName)
                                .FirstOrDefault();

            return _class?
                        .Methods
                        .Where(c => c.Name == ".ctor")
                        .FirstOrDefault();
        }

        private Stfld ctorValues(Instruction inst)
        {
            switch(((FieldDefinition)inst.Operand).Name)
            {
                case "<CallingConvention>k__BackingField":
                {
                    const string pname = "CallingConvention";
                    object val = null;

                    if(inst.Previous.Operand != null) {
                        val = inst.Previous.Operand;
                    }
                    else if(inst.Previous.OpCode.OperandType == OperandType.InlineNone
                                    && inst.Previous.OpCode.OpCodeType == OpCodeType.Macro)
                    {
                        val = inst.Previous.OpCode.Op2 - 0x16;
                    }

                    return new Stfld(pname, val);
                }
                case "<ExportName>k__BackingField": {
                    return new Stfld("ExportName", inst.Previous.Operand);
                }
            }

            return new Stfld(null, null);
        }
    }
}
