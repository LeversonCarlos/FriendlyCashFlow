import { Injectable } from '@angular/core';
import { BaseRequest, BaseResponse, ApiClient } from '@components/api-client';
import { IRepository } from '@interfaces/IRepository';
import { AccountModel, BackendRoute } from '@models/accounts';

@Injectable()
export class SearchRepository implements IRepository<string, AccountModel[]> {

   constructor(
      private apiClient: ApiClient,
   ) { }

   public async Handle(searchTerms: string): Promise<AccountModel[]> {

      const request: SearchRequestModel = {
         Url: `${BackendRoute}/search`,
         SearchTerms: searchTerms,
      };

      const response = await this.apiClient.Handle<SearchResponseModel>(request);

      if (!response || !response.Accounts)
         return [];

      const result = response.Accounts
         .map(x => AccountModel.Parse(x));

      return result;
   }

}

class SearchRequestModel extends BaseRequest {
   SearchTerms: string = '';
}

class SearchResponseModel extends BaseResponse {
   Accounts?: AccountModel[];
}