import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { CommonSecureModule } from '../common/common.module';

import { EntriesRouting } from './entries.routing';
import { EntriesComponent } from './entries.component';
import { EntriesFlowComponent } from './entries-flow/entries-flow.component';
import { EntriesResumeComponent } from './entries-resume/entries-resume.component';
import { SearchPanelComponent } from './search-panel/search-panel.component';
import { EntryDetailsComponent } from './entry-details/entry-details.component';
import { TransferDetailsComponent } from './transfer-details/transfer-details.component';
import { EntriesService } from './entries.service';
import { TransfersService } from '../transfers/transfers.service';
import { PatternsService } from '../patterns/patterns.service';
import { RecurrencyService } from '../recurrency/recurrency.service';

@NgModule({
   declarations: [EntriesComponent, EntriesFlowComponent, EntriesResumeComponent, SearchPanelComponent,
      EntryDetailsComponent, TransferDetailsComponent],
   imports: [
      CommonModule, SharedModule, CommonSecureModule,
      EntriesRouting
   ],
   providers: [EntriesService, TransfersService, PatternsService, RecurrencyService]
})
export class EntriesModule { }
