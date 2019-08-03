import { Pipe, PipeTransform } from '@angular/core';
import { TranslationService } from './translation.service';

@Pipe({
   name: 'translation'
})
export class TranslationPipe implements PipeTransform {

   constructor(private service: TranslationService) { }

   async transform(val: string): Promise<string> {
      return this.service.getValue(val);
   }

}
