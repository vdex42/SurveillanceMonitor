using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SurveillanceMonitor.Infrastructure.Config;

namespace SurveillanceMonitor.Services
{
    public class AlarmHandlers : IAlarmHandlers
    {
        private readonly Dictionary<int, List<IAlarmHandler>> _handlers;
        private DateTime? _lastAlarm;

        //Constructor 
        public AlarmHandlers()
        {
            //initialize generic Collection(Composition)
            _handlers =  new Dictionary<int,List<IAlarmHandler>>();
        }
        //Adds the handler to the composition
        public void Add(IAlarmHandler handler, SurveillanceMonitorConfig.SurveillanceMonitorCamera camera)
        {
            if(!_handlers.ContainsKey(camera.Id))
                _handlers.Add(camera.Id, new List<IAlarmHandler>());

            _handlers[camera.Id].Add(handler);
        }
        
        //Removes the handler from the composition
        public void Delete(IAlarmHandler handler)
        {
            foreach (var key in _handlers.Keys)
            {
                _handlers[key].Remove(handler);
            }
            
        }
        
        public void AlarmActivated(SurveillanceMonitorConfig.SurveillanceMonitorCamera camera)
        {
            //Dedupe duplicate(ish) alarm notifications
            if (_lastAlarm.HasValue)
            {
                if (DateTime.Now.Subtract(_lastAlarm.Value).Seconds < 10)
                {
                    return;
                }
            }
            _lastAlarm = DateTime.Now;

            foreach (var handler in _handlers[camera.Id])
            {
                if (!handler.IsHandlerBusy)
                    Task.Run(() => handler.AlarmActivated());
                else
                    Task.Run(() => handler.ExtendAlarm());
            }
        }
    }
}
