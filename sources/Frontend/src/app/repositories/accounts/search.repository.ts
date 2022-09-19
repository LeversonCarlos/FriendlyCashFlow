import { Injectable } from '@angular/core';
import { BaseRequest, BaseResponse, ApiClient } from '@components/api-client';
import { AccountModel, ApiUrl } from '@models/accounts';

@Injectable()
export class SearchRepository {

   constructor(
      private apiClient: ApiClient,
   ) { }

   public async Handle(searchTerms: string): Promise<AccountModel[] | null> {

      const request: SearchRequestModel = {
         Url: `${ApiUrl}/search`,
         SearchTerms: searchTerms,
      };

      const response = await this.apiClient.Handle<SearchResponseModel>(request);

      if (!response || !response.Accounts)
         return null;

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
