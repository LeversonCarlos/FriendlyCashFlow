import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { TokenService } from '../token/token.service';
import { AuthGuard } from './auth.guard';

describe('AuthGuard', () => {

   let router: Router;
   let tokenService: TokenService;
   let guard: AuthGuard;

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
      guard = TestBed.inject(AuthGuard);
   });

   it('should be created', () => {
      expect(guard).toBeTruthy();
   });

});
