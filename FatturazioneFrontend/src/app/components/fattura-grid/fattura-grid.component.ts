import { Component, Input } from '@angular/core';
import { FatturaService } from '../../services/fattura.service';
import { FatturaDataService } from '../../services/fattura-data.service';

@Component({
  selector: 'app-fattura-grid',
  templateUrl: './fattura-grid.component.html',
  styleUrls: ['./fattura-grid.component.css']
})
export class FatturaGridComponent {
  @Input() righe: any[] = [];
  displayedColumns: string[] = ['desc', 'cfact', 'preciob', 'precion', 'neto'];
  testataFilePath: string = '';
  righeFilePath: string = '';
  year: number | null = null;

  constructor(
    private fatturaService: FatturaService,
    private fatturaDataService: FatturaDataService
  ) {}

  onGenerateFileFatturazione() {
    const donumdoc = this.fatturaDataService.getDonumdoc();
    const year = this.fatturaDataService.getYear();
    if (!donumdoc.trim()) {
      alert("Inserisci un numero di fattura valido.");
      return;
    }

    const yearParam = year !== null ? year : undefined;

    this.fatturaService.generaFileFatturazione(donumdoc, yearParam).subscribe(
      (response: any) => {
        console.log("Risposta del backend:", response)
        if (response && response.testataFilePath && response.righeFilePath) {
          this.testataFilePath = response.testataFilePath;
          this.righeFilePath = response.righeFilePath;
          alert(`File generati con successo:\n\n`
           + `Percorso file di testata: \n${this.testataFilePath}\n\n`
           + `Percorso file di riga: \n${this.righeFilePath}`);
        } else {
        alert('Errore nella generazione dei file: dati incompleti ricevuti dal server.');
        }
      },
      (error: any) => {
        console.error("Errore durante la generazione dei file di fatturazione:", error);
        if (error.status === 400) {
          if (error.error === "Occorre specificare l'anno del documento per generare il documento") {
          alert ("Per generare il file di questa fattura occorre specificare anche l'anno di fatturazione.");
        } else {
          alert("Errore durante la generazione dei file di fatturazione: " + error.error);
        } 
      } else if (error.status === 409) {
          alert("Si è verificato un errore durante la generazione dei file di fatturazione: Non è possibile generare file duplicati. Per favore, riprova più tardi.");
        } else {
          alert("Si è verificato un errore durante la generazione dei file di fatturazione. Per favore, riprova più tardi.");
        }
      }
    );
  }

  updateDonumdoc(value: string) {
    this.fatturaDataService.setDonumdoc(value);
  }

  updateYear(value: number | null) {
    this.fatturaDataService.setYear(value);
    this.year = value;
  }
}
