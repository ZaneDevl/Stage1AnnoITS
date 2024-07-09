import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-fattura-display',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fattura-display.component.html',
  styleUrl: './fattura-display.component.css'
})
export class FatturaDisplayComponent {
  @Input() fattura: any;
}
