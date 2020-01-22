import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { environment } from 'src/environments/environment';
import { Balance } from 'src/app/secure/dashboards/dashboards.viewmodels';
import { DashboardsService } from 'src/app/secure/dashboards/dashboards.service';

@Component({
   selector: 'fs-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

   constructor(public auth: AuthService, private dashboardsService: DashboardsService) { }

   public appVersion: string = environment.appVersion
   public balanceList: Balance[]

   public async ngOnInit() {
      this.balanceList = await this.dashboardsService.getBalances();
   }

}
