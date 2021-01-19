import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEntity } from '../../categories.data';

@Component({
   selector: 'categories-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor() { }

   @Input()
   public Categories: Observable<CategoryEntity[]>

   ngOnInit(): void {
   }

}
