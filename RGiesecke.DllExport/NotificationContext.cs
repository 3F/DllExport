//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport
{
    public sealed class NotificationContext: IEquatable<NotificationContext>
    {
        public string Name
        {
            get;
            private set;
        }

        public object Context
        {
            get;
            private set;
        }

        public NotificationContext(string name, object context)
        {
            this.Name = name;
            this.Context = context;
        }

        public static bool operator ==(NotificationContext left, NotificationContext right)
        {
            return object.Equals((object)left, (object)right);
        }

        public static bool operator !=(NotificationContext left, NotificationContext right)
        {
            return !object.Equals((object)left, (object)right);
        }

        public bool Equals(NotificationContext other)
        {
            if(object.ReferenceEquals((object)null, (object)other))
            {
                return false;
            }
            if(object.ReferenceEquals((object)this, (object)other))
            {
                return true;
            }
            if(object.Equals(this.Context, other.Context))
            {
                return string.Equals(this.Name, other.Name);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if(object.ReferenceEquals((object)null, obj))
            {
                return false;
            }
            if(object.ReferenceEquals((object)this, obj))
            {
                return true;
            }
            if(obj is NotificationContext)
            {
                return this.Equals((NotificationContext)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (this.Context != null ? this.Context.GetHashCode() : 0) * 397 ^ (this.Name != null ? this.Name.GetHashCode() : 0);
        }
    }
}
