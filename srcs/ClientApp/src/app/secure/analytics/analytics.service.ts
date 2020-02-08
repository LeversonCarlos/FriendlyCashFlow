import { Injectable } from '@angular/core';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class AnalyticsService {

   constructor(private busy: BusyService, private http: HttpClient) { }

}
