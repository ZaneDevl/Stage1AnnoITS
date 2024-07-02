import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FatturaService } from '../../services/fattura.service';
import { FatturaDataService } from '../../services/fattura-data.service';

@Component({
  selector: 'app-fattura-grid',
  //standalone: true,
  //imports: [MatTableModule, MatCardModule, MatButtonModule, CommonModule, CurrencyPipe],
  templateUrl: './fattura-grid.component.html',
  styleUrls: ['./fattura-grid.component.css']
})
export class FatturaGridComponent implements OnInit{
  
  @Input() righe: any[] = [];
  displayedColumns: string[] = ['desc', 'cfact', 'preciob', 'precion', 'neto'];

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['righe']) {
      console.log('Dati delle righe aggiornati:', this.righe)
    }
  }

constructor (
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
        alert(`File generati con successo:\nTestata: ${response.TestataFilePath}\nRighe: ${response.RigheFilePath}`);
      },
      (error: any) => {
        console.error("Errore durante la generazione dei file di fatturazione:", error);
        alert("Si è verificato un errore durante la generazione dei file di fatturazione. Per favore, riprova.");
      }
    );
  }
}

  

