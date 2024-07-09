import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FatturaGridModule } from '../components/fattura-grid/fattura-grid.module';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { FatturaService } from '../services/fattura.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    MatCardModule,
    MatTableModule,
    FatturaGridModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule
  ],
  providers: [FatturaService],
  bootstrap: [AppComponent]
})
export class AppModule { }
