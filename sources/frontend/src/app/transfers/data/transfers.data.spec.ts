import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, Params, Route } from '@angular/router';
import { TestsModule } from '@elesse/tests';
import { TransfersData } from './transfers.data';

describe('TransfersData', () => {
   let service: TransfersData;
   let httpMock: HttpTestingController;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(TransfersData);
      httpMock = TestBed.inject(HttpTestingController);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});

export function getSnapshot(path: string = null, params: Params = null): ActivatedRouteSnapshot {
   let routeConfig: Route = null;
   if (path != null)
      routeConfig = { path: path }
   const snapshot: ActivatedRouteSnapshot = { url: null, routeConfig: routeConfig, params: params, queryParams: null, fragment: null, data: null, outlet: null, component: null, root: null, parent: null, firstChild: null, children: null, pathFromRoot: null, paramMap: null, queryParamMap: null };
   return snapshot;
}
