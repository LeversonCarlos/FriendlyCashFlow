import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { SettingsService } from '../settings/settings.service';

enum SeverityLevel {
   Verbose = 0,
   Information = 1,
   Warning = 2,
   Error = 3,
   Critical = 4,
}

@Injectable({
   providedIn: 'root'
})
export class InsightsService {

   constructor(private settings: SettingsService) {
      try {
         if (settings == undefined || settings == null)
            return;

         this.settings
            .getSettings()
            .subscribe(cfg => {

               if (!cfg || !cfg.AppInsights)
                  return;
               if (!cfg.AppInsights.Activated)
                  return;
               if (!cfg.AppInsights.Key || cfg.AppInsights.Key == '')
                  return;

               this.AppInsights = new ApplicationInsights({
                  config: {
                     instrumentationKey: cfg.AppInsights.Key,
                     samplingPercentage: 100,
                     enableAutoRouteTracking: true
                  }
               });
               this.AppInsights.loadAppInsights();

            });

      }
      catch (ex) { console.error(ex); }
   }

   private AppInsights: ApplicationInsights;

   public TrackEvent(name: string, properties?: any) {
      if (this.AppInsights)
         this.AppInsights.trackEvent({ name }, properties)
   }

   public TrackError(error: Error) {
      if (this.AppInsights)
         this.AppInsights.trackException({ error, severityLevel: SeverityLevel.Critical })
   }

   /*
   public TrackTrace(message: string, severityLevel: SeverityLevel = SeverityLevel.Information, properties?: any) {
      if (this.AppInsights)
         this.AppInsights.trackTrace({ message, severityLevel }, properties)
   }
   */

   public TrackMetric(name: string, value?: number, min?: number, max?: number) {
      if (this.AppInsights)
         this.AppInsights.trackMetric({ name, average: value, min, max })
   }

}
