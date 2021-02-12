import { TestBed } from '@angular/core/testing';
import { BusyService } from './busy.service';

describe('BusyService', () => {
   let service: BusyService;

   beforeEach(() => {
      TestBed.configureTestingModule({});
      service = TestBed.inject(BusyService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('Show_Must_SetBusyToTrue_AndEmitTrueOnBusyChange', () => {
      service.hide();
      spyOn(service.BusyChange, 'emit');
      service.show();
      expect(service.IsBusy).toBeTrue();
      expect(service.BusyChange.emit).toHaveBeenCalledWith(true);
   });

   it('Hide_Must_SetBusyToFalse_AndEmitFalseOnBusyChange', () => {
      service.show();
      spyOn(service.BusyChange, 'emit');
      service.hide();
      expect(service.IsBusy).toBeFalse();
      expect(service.BusyChange.emit).toHaveBeenCalledWith(false);
   });

});
