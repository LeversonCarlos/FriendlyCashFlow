import { Injectable } from '@angular/core';
import { SwUpdate, UpdateAvailableEvent } from '@angular/service-worker';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslationService } from 'src/app/shared/translation/translation.service';

@Injectable({
   providedIn: 'root'
})
export class ServiceWorkerUpdateService {

   constructor(private appInsights: AppInsightsService, private translationService: TranslationService,
      private swUpdate: SwUpdate, private snackbar: MatSnackBar) {
      this.swUpdate.available.subscribe(evt => { this.showSnack(evt); });
   }

   private async showSnack(evt: UpdateAvailableEvent) {
      try {
         this.appInsights.trackEvent("Version Update Available", [`currentVersion:${evt.current}`, `availableVersion:${evt.available}`])

         const messageText = await this.translationService.getValue('SERVICE_WORKER_UPDATE_AVAILABLE_INFO');
         const commandText = await this.translationService.getValue('SERVICE_WORKER_UPDATE_AVAILABLE_COMMAND');
         this.snackbar
            .open(messageText, commandText, { duration: 6000 })
            .onAction()
            .subscribe(() => {
               window.location.reload();
            });
      }
      catch (ex) { console.error(ex) }
   }

}
