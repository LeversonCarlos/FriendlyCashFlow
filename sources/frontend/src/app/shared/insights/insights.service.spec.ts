import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { InsightsService } from './insights.service';

describe('InsightsService', () => {
   let service: InsightsService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [HttpClientTestingModule]
         /*
         providers: [
            {
               provide: SettingsService,
               useValue: jasmine.createSpyObj('SettingsService', ['getSettings'])
            }
         ]
         */
      });
      service = TestBed.inject(InsightsService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
