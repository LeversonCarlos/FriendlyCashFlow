import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { CommonSecureModule } from './common/common.module';

import { EntriesService } from './entries/entries.service';
import { PatternsService } from './patterns/patterns.service';
import { RecurrencyService } from './recurrency/recurrency.service';
import { TransfersService } from './transfers/transfers.service';
import { EntriesFlowComponent } from './entries/entries-flow/entries-flow.component';
import { EntryDetailsComponent } from './entries/entry-details/entry-details.component';
import { TransferDetailsComponent } from './entries/transfer-details/transfer-details.component';
import { EntriesResumeComponent } from './entries/entries-resume/entries-resume.component';
import { SearchPanelComponent } from './entries/search-panel/search-panel.component';

@NgModule({
   declarations: [
      EntriesFlowComponent, EntryDetailsComponent, TransferDetailsComponent, EntriesResumeComponent, SearchPanelComponent],
   imports: [
      CommonModule, SharedModule, CommonSecureModule
   ],
   providers: [EntriesService, TransfersService, PatternsService, RecurrencyService]
})
export class SecureModule { }
