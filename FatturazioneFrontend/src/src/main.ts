import { platformBrowser } from '@angular/platform-browser';
import { enableProdMode } from '@angular/core';
import { AppModule } from './app/app/app.module';
import { environment } from './environment';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';

if (environment.production) {
  enableProdMode();
}

platformBrowser().bootstrapModule(AppModule) // Bootstrap dell'AppModule
  .then((moduleRef) =>{
    return bootstrapApplication(AppComponent)
  })
  .catch((err: any) => console.error(err));