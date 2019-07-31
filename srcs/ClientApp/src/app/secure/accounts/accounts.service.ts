import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export class Account {
   AccountID: number;
   Text: string;
   Type: number;
   DueDay?: number;
   Active: boolean;
}

@Injectable({
   providedIn: 'root'
})
export class AccountsService {

   constructor(private http: HttpClient) { }

   public async getAccounts(): Promise<Account[]> {
      try {
         const dataList = await this.http.get<Account[]>("api/accounts").toPromise();
         console.log(dataList)
         return dataList;
      }
      catch (ex) { console.error(ex); return null; }

   }

}
