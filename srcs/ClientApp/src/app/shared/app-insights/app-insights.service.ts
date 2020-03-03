import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { AppSettingsService } from '../app-settings/app-settings.service';

class AppSettingsConfig {
   AppInsights: AppInsightsConfig
}

class AppInsightsConfig {
   Activated: boolean
   Key: string
}

export enum SeverityLevel {
   Verbose = 0,
   Information = 1,
   Warning = 2,
   Error = 3,
   Critical = 4,
}

@Injectable({
   providedIn: 'root'
})
export class AppInsightsService {

   constructor(private appSettings: AppSettingsService) {
      try {
         this.appSettings
            .getSettings()
            .subscribe((cfg: AppSettingsConfig) => {
               console.log('appSettings', cfg)
               if (!cfg || !cfg.AppInsights) { return; }
               if (!cfg.AppInsights.Activated) { return; }
               if (!cfg.AppInsights.Key || cfg.AppInsights.Key == '') { return; }

               this.AppInsights = new ApplicationInsights({
                  config: {
                     instrumentationKey: cfg.AppInsights.Key,
                     samplingPercentage: 100,
                     enableAutoRouteTracking: true
                  }
               });
               this.AppInsights.loadAppInsights();
            })
      }
      catch{ }
   }
   private AppInsights: ApplicationInsights

   public trackPageView(name: string, uri: string) {
      if (this.AppInsights) {
         this.AppInsights.trackPageView({ name, uri })
      }
   }

   public trackEvent(name: string, properties?: any) {
      if (this.AppInsights) {
         this.AppInsights.trackEvent({ name }, properties)
      }
   }

   public trackException(exception: Error) {
      if (this.AppInsights) {
         this.AppInsights.trackException({ exception, severityLevel: SeverityLevel.Critical })
      }
   }

   public trackTrace(message: string, severityLevel: SeverityLevel = SeverityLevel.Information, properties?: any) {
      if (this.AppInsights) {
         console.log('trackTrace', { message, severityLevel, properties })
         this.AppInsights.trackTrace({ message, severityLevel }, properties)
      }
   }

   public trackMetric(name: string, value?: number, min?: number, max?: number) {
      if (this.AppInsights) {
         this.AppInsights.trackMetric({ name, average: value, min, max })
      }
   }

}
