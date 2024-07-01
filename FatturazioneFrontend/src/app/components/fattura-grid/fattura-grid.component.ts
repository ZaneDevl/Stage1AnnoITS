import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';

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
}
