import { Pipe, PipeTransform } from '@angular/core';
import { LocalizationService } from './localization.service';

@Pipe({
   name: 'translation'
})
export class TranslationPipe implements PipeTransform {

   constructor(private service: LocalizationService) { }

   transform(value: string): Promise<string> {
      return this.service.GetTranslation(value);
   }

}
