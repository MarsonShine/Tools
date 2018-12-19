using EasyQuartz.JobFactories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyQuartz
{
    public class BusinessLayer
    {
        private readonly InjectionableJobFactory m_factory;

        public BusinessLayer(InjectionableJobFactory factory)
        {
            m_factory = factory;
        }


    }
}
