import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { MessageService } from './message.service';

describe('MessageService', () => {
   let service: MessageService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(MessageService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
