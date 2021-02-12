// export type TranslationValues = { [id: string]: string; }
export type TranslationValues = Record<string, string>

export class TranslationData {
   Version: string
   Language: string
   Values: TranslationValues
}
