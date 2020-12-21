import { TestBed } from '@angular/core/testing';
import { SettingsService } from '../settings/settings.service';

import { InsightsService } from './insights.service';

describe('InsightsService', () => {

   let settingsService: SettingsService;
   let service: InsightsService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         providers: [
            {
               provide: SettingsService,
               useValue: jasmine.createSpyObj('SettingsService', ['getSettings'])
            }
         ]
      });
      settingsService = TestBed.inject(SettingsService);
      service = TestBed.inject(InsightsService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
