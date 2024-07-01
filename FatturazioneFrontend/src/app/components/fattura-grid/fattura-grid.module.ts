import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FatturaGridComponent } from './fattura-grid.component';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';



@NgModule({
    declarations: [
        FatturaGridComponent
    ],
    imports : [
        CommonModule,
        MatTableModule,
        MatButtonModule,
        MatCardModule
    ],
    exports: [
        FatturaGridComponent
    ]
})
export class FatturaGridModule { }