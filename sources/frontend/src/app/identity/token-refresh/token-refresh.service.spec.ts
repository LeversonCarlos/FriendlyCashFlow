import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { TokenRefreshService } from './token-refresh.service';

describe('TokenRefreshService', () => {

   let service: TokenRefreshService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [HttpClientTestingModule]
      });
      service = TestBed.inject(TokenRefreshService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
