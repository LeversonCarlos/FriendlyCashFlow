import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
   selector: 'fs-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

   constructor(private http: HttpClient) { }
   public Data: any;

   public async ngOnInit() {
      this.Data = await this.http.get<any>("api/SampleData/WeatherForecasts").toPromise();
   }

}
