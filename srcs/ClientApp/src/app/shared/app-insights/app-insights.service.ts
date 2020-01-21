import { Injectable } from '@angular/core';
import { ApplicationInsights, IEventTelemetry, IExceptionTelemetry, ITraceTelemetry, IMetricTelemetry } from '@microsoft/applicationinsights-web';
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
            .toPromise<AppSettingsConfig>()
            .then(cfg => {
               if (!cfg || !cfg.AppInsights) { return; }
               if (!cfg.AppInsights.Activated) { return; }
               if (!cfg.AppInsights.Key || cfg.AppInsights.Key == '') { return; }

               this.AppInsights = new ApplicationInsights({
                  config: {
                     instrumentationKey: cfg.AppInsights.Key,
                     enableAutoRouteTracking: true
                  }
               });
               this.AppInsights.loadAppInsights();
            });
      }
      catch{ }
   }
   private AppInsights: ApplicationInsights

   public trackPageView(name: string, uri: string) {
      this.AppInsights?.trackPageView({ name, uri })
   }

   public trackEvent(name: string, properties?: any) {
      this.AppInsights?.trackEvent({ name }, properties)
   }

   public trackException(exception: Error) {
      this.AppInsights?.trackException({ exception, severityLevel: SeverityLevel.Critical })
   }

   public trackTrace(message: string, severityLevel: SeverityLevel = SeverityLevel.Information, properties?: any) {
      this.AppInsights?.trackTrace({ message, severityLevel }, properties)
   }

   public trackMetric(name: string, value?: number, min?: number, max?: number) {
      this.AppInsights?.trackMetric({ name, average: value, min, max })
   }

}
