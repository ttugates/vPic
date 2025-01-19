import { Component } from '@angular/core';
// import { RouterOutlet } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon'
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [
    //RouterOutlet, 
    MatCardModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatIconModule, 
    MatButtonModule,
    FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'vpic';

  public vinInput: string | null = null;
  public isVinMinimallyValid = false;

  public vinDecodedSpecs: string | null = null;

  public isValidNorthAmericanVIN(val: string | null): boolean {
    return val?.length === 17;
  }

  public validateVinInput(e: string | null): void {
    this.isVinMinimallyValid = this.isValidNorthAmericanVIN(e);
  }

  public onClickTestInsertVIN(){
    this.vinInput = '1GKKNPLS3MZ188177';
    this.validateVinInput(this.vinInput);
  }

  public onClickDecodeVIN(){
    const url = `http://localhost:5042/vpic/vehicle_specifications/${this.vinInput}`;
    fetch(url)
    .then(async r =>{
      if(r.ok){        
        this.vinDecodedSpecs = await r.json();
        console.log(this.vinDecodedSpecs);
      } else {
        console.log(r.status, r.statusText);
        this.vinDecodedSpecs = null;
      }
    })
    .catch(e => {
      this.vinDecodedSpecs = null;
      console.error(e); 
    });
  }

}
