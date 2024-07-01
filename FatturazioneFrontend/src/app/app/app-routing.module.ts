import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { FatturaGridComponent } from "../components/fattura-grid/fattura-grid.component";

const routes : Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'home', component: FatturaGridComponent },
    {path: '**', redirectTo: '/home'}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}