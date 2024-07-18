import { NgIf } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-fattura-form',
  standalone: true,
  imports: [NgIf, FormsModule],
  templateUrl: './fattura-form.component.html',
  styleUrl: './fattura-form.component.css'
})
export class FatturaFormComponent {
  fatturaNum: string = '';
  year: number | null = null;
  error: string = '';
  @Output() submitFattura = new EventEmitter<{ fatturaNum: string, year: number | null }>();


  handleSubmit() {
    if (!this.fatturaNum) {
      this.error = 'Inserisci un numero di fattura';
    } else {
      this.error = '';
      this.submitFattura.emit({ fatturaNum: this.fatturaNum, year: this.year});
    }
  }
}
