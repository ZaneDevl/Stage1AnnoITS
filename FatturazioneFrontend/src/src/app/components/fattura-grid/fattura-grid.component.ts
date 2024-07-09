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

  constructor(
    private fatturaService: FatturaService,
    private fatturaDataService: FatturaDataService
  ) {}

  onGenerateFileFatturazione() {
    const donumdoc = this.fatturaDataService.getDonumdoc();
    if (!donumdoc.trim()) {
      alert("Inserisci un numero di fattura valido.");
      return;
    }

    this.fatturaService.generaFileFatturazione(donumdoc).subscribe(
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
        if (error.status === 409) {
          alert("Si è verificato un errore durante la generazione dei file di fatturazione: Non è possibile generare file duplicati. Per favore, riprova più tardi.");
        } else {
          alert("Si è verificato un errore durante la generazione dei file di fatturazione. Per favore, riprova più tardi.");
        }
      }
    );
  }
}
