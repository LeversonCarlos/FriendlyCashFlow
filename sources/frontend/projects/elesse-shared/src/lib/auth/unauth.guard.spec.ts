import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { TokenService } from './token.service';
import { UnauthGuard } from './unauth.guard';

describe('UnauthGuard', () => {

   let router: Router;
   let tokenService: TokenService;
   let guard: UnauthGuard;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [
            RouterTestingModule.withRoutes([]),
         ],
         providers: [
            {
               provide: TokenService,
               useValue: jasmine.createSpyObj('TokenService', ['HasToken'])
            }
         ]
      });
      router = TestBed.inject(Router);
      tokenService = TestBed.inject(TokenService);
      guard = TestBed.inject(UnauthGuard);
   });

   it('should be created', () => {
      expect(guard).toBeTruthy();
   });

});
