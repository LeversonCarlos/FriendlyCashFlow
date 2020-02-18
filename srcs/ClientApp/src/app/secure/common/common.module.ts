import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountPickerComponent } from './account-picker/account-picker.component';
import { MonthPickerComponent } from './month-picker/month-picker.component';
import { CommonResumeComponent } from './resume/resume.component';
import { AddButtonComponent } from './add-button/add-button.component';

@NgModule({
   declarations: [AccountPickerComponent, MonthPickerComponent, CommonResumeComponent, AddButtonComponent],
   imports: [CommonModule, SharedModule],
   exports: [AccountPickerComponent, MonthPickerComponent, CommonResumeComponent, AddButtonComponent]
})
export class CommonSecureModule { }
