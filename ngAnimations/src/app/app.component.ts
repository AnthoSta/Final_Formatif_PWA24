import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, flash, shake, shakeX, tada } from 'ng-animate';
import { delay } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    animations:[
      trigger("bounce", [transition(":increment", useAnimation(bounce, {
        params:{timing:4, delay:2}
      }))]),
      trigger("shake", [transition(":increment", useAnimation(shake, {
        params:{timing:2, delay:0}
      }))]),
      trigger("tada", [transition(":increment", useAnimation(tada, {
        params:{timing:3, delay:5}
      }))])
    ],
    styleUrls: ['./app.component.css'],
    standalone: true
})
export class AppComponent {
  title = 'ngAnimations';

  bounce = 0;
  shake = 0;
  tada = 0
  constructor() {
  }

  oneTime(boucleValue:boolean){
    console.log(boucleValue)
    this.shake++;
    this.bounce++;
    this.tada++;
    setTimeout(() => {if(boucleValue){
      this.oneTime(true)
    }}, 8000);
    
    
  }

  rotate(){
    var element = document.getElementById("turnId")
    element?.classList.add("rotate-left")
    setTimeout(() => {
      element?.classList.remove("rotate-left")
    }, 2000);

  }
}
