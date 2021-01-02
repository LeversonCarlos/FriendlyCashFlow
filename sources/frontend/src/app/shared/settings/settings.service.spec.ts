import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { SettingsService } from './settings.service';

describe('SettingsService', () => {
   let service: SettingsService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [HttpClientModule]
      });
      service = TestBed.inject(SettingsService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   /*
   TODO
   it('getPrinter_WithValidParameter_MustResultExpectedData',
      inject([HttpTestingController, PrintersSelectorService],
         (httpMock: HttpTestingController, service: PrintersSelectorService) => {

            // expected data
            const expectedData = Object.assign(new Printer, { PrinterID: 1, PrinterCode: 'PRT01' });

            // service execution
            service.getPrinter(expectedData.PrinterID).subscribe(data => {
               expect(data.PrinterID).toBe(expectedData.PrinterID);
               expect(data.PrinterCode).toBe(expectedData.PrinterCode);
            });

            // set the expectations for the HttpClient mock
            const req = httpMock.expectOne(`${service.BaseUrl}/item/${expectedData.PrinterID}`);
            expect(req.request.method).toEqual('GET');

            // set the fake data to be returned by the mock
            req.flush(expectedData);

         })
   );
   */

});
