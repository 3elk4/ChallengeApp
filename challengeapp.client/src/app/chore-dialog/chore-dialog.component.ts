import { Component, Inject, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Chore } from "../../services/chores/dto/chore";
import { ChoreDifficulty } from "../../services/chores/dto/chore-difficulty";

@Component({
  selector: 'chore-dialog',
  templateUrl: './chore-dialog.component.html'
})
export class ChoreDialogComponent implements OnInit {
  form1: FormGroup = new FormGroup({ title: new FormControl(''), description: new FormControl(''), points: new FormControl(1), difficulty: new FormControl(0) });

  title: string;
  description: string;
  points: number;
  difficulty: number;

  StringIsNumber = (value: string) => isNaN(Number(value)) === false;

  choreDifficultyTypes = Object.keys(ChoreDifficulty).filter(this.StringIsNumber).map(
    (type) => ({ key: type, value: (ChoreDifficulty as any)[type] })
  );

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ChoreDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.title = data.title;
    this.description = data.description;
    this.points = data.points;
    this.difficulty = data.dificulty;
  }

  ngOnInit() {
    this.form1 = this.fb.group({
      title: [this.title, [Validators.required, Validators.maxLength(250)]],
      description: [this.description, [Validators.required, Validators.maxLength(1000)]],
      points: [this.points, [Validators.required, Validators.min(1)]],
      difficulty: [this.difficulty, [Validators.required]]
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form1.controls;
  }

  save() {
    if (this.form1.invalid) {
      return;
    }

    this.f['difficulty'].setValue(parseInt(this.f['difficulty'].value));

    this.dialogRef.close(this.form1.value as Chore);
  }

  close() {
    this.dialogRef.close(null);
  }
}
