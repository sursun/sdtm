using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    public abstract class Completion : Entity
    {
        public virtual int Total { get; set; }
        public virtual int Completed { get; set; }
      
        public virtual void Computer()
        {
            this.Total = 0;
            this.Completed = 0;

            
            Type type = this.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo p in props)
            {
                object[] attrs = p.GetCustomAttributes(typeof(FieldNeedAttribute), false);
                if (attrs.Length != 0)
                {
                    this.Total++;

                    if (((FieldNeedAttribute)attrs[0]).DefaultValue == null)
                    {
                        if (p.GetValue(this, null) != null)
                        {
                            this.Completed++;
                            //Console.WriteLine(string.Format("PropertyInfo--{0}-{1}", p.Name, p.GetValue(this, null)));
                        }
                        //else
                        //{
                        //    Console.WriteLine("PropertyInfo---:" + p.Name);
                        //}
                    }
                    else
                    {
                        Type attrType = p.PropertyType;
                        Object val = ((FieldNeedAttribute)attrs[0]).DefaultValue;
                        Object objVal = p.GetValue(this, null);


                        if (attrType.FullName.Equals("System.DateTime"))
                        {
                            int year = (int)val;
                            DateTime timeDest = (DateTime)objVal;

                            if (year != timeDest.Year)
                            {
                                this.Completed++;
                            }

                           // Console.WriteLine("PropertyInfo DateTime:" + p.Name);
                        }
                        //else if (attrType.FullName.Contains("System.Collections.Generic.IList"))
                        //{
                        //    int year = (int)val;
                        //    IList<> timeDest = (IList<>)objVal;

                        //    if (year != timeDest.Year)
                        //    {
                        //        this.Completed++;
                        //    }

                        //    // Console.WriteLine("PropertyInfo DateTime:" + p.Name);
                        //}
                        else
                        {
                           // Console.WriteLine(string.Format("PropertyInfo {0}-{1}", attrType.FullName, p.Name));

                            if (!val.Equals(objVal))
                            {
                                this.Completed++;
                            }
                        }
                    }
                }
            }
        }

        public virtual int Process()
        {
            if (Total < 1)
            {
                return 0;
            }

            return Completed*100/Total;
        }
    }
}
