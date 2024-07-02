import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { FatturaService } from './services/fattura.service';  
import { ErrorMessageComponent } from './components/error-message/error-message.component';
import { FormsModule } from '@angular/forms';
import { FatturaGridModule } from './components/fattura-grid/fattura-grid.module';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { FatturaDataService } from './services/fattura-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'FattureApp';
  fattura: any = null;
  donumdoc: string = '';
  error: string = '';
  notification: string = '';
  isLoading: boolean = false;

  constructor(
    private fatturaService: FatturaService,
    private fatturaDataService: FatturaDataService
  ) {}

  fetchFattura() {
    if (!this.donumdoc.trim()) {
      this.notification = 'Inserisci un numero di fattura';
      this.clearData();
      return;
    }
    
    this.fatturaDataService.setDonumdoc(this.donumdoc);
    
    this.isLoading=true;
    this.notification='';
    this.error = '';

    this.fatturaService.getFattura(this.donumdoc).subscribe(
      (data) => {
        this.isLoading = false;
        if (!data) {
          this.notification = 'Fattura non trovata';
          this.clearData();
        } else {
          this.fattura = data;
          this.notification = '';
          console.log('Dati fattura ricevuti:', this.fattura);
        }     
      },
      (err) => {
        this.isLoading = false;
        this.notification = 'Errore nel recupero della fattura';
        this.clearData();
        console.error('Errore:', err);
      }
    );
  }
  
  generateInvoiceFile() {
    if (!this.donumdoc.trim()) {
      this.notification = 'Inserisci un numero di fattura valido';
      return;
    }
  }

  clearData() {
    this.fattura = null;
    this.isLoading = false;
  }
}


