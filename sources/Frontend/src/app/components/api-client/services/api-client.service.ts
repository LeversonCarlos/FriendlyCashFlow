import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService } from '@components/busy';
import { BaseRequest, BaseResponse } from '..';

@Injectable()
export class ApiClient {

   constructor(
      private busy: BusyService,
      private http: HttpClient,
   ) { }

   public async Handle<T extends BaseResponse>(request: BaseRequest): Promise<T> {
      try {
         this.busy.show();

         const response = await this.http.post<T>(request.Url, request).toPromise();

         if (!response)
            throw new Error(`Error: Invalid null response from ${request.Url}`);

         if (!response.OK && (!response.Messages || response.Messages.length == 0))
            throw new Error(`Error: Invalid response without proper messages from ${request.Url}`);

         return response;

      }
      catch (err) { return BaseResponse.CreateError<T>(err); }
      finally { this.busy.hide(); }
   }

}
