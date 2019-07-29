import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
   declarations: [HomeComponent],
   imports: [
      CommonModule, HttpClientModule
   ],
   exports: [HttpClientModule]
})
export class CoreModule { }
