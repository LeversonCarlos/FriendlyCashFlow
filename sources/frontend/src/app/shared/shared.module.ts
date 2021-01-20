import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BusyComponent } from './busy/busy.component';
import { AnonymousContainerComponent } from './containers/anonymous-container/anonymous-container.component';
import { AuthenticatedContainerComponent } from './containers/authenticated-container/authenticated-container.component';
import { MaterialModule } from '@elesse/material';
import { LinksComponent } from './containers/links/links.component';
import { MessageViewComponent } from './message/message-view/message-view.component';
import { ConfirmViewComponent } from './message/confirm-view/confirm-view.component';
import { VersionComponent } from './version/version.component';
import { TranslationPipe } from './localization/translation.pipe';
import { RelatedboxComponent } from './relatedbox/relatedbox.component';
import { EmptyListComponent } from './empty-list/empty-list.component';

@NgModule({
   entryComponents: [MessageViewComponent],
   declarations: [
      BusyComponent,
      AnonymousContainerComponent, AuthenticatedContainerComponent, LinksComponent,
      MessageViewComponent, ConfirmViewComponent, VersionComponent,
      TranslationPipe, RelatedboxComponent, EmptyListComponent
   ],
   imports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      MaterialModule
   ],
   exports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      BusyComponent,
      AnonymousContainerComponent, AuthenticatedContainerComponent,
      TranslationPipe, RelatedboxComponent, EmptyListComponent
   ]
})
export class SharedModule { }
