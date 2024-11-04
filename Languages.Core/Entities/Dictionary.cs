using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Definition
    {
        [Key]
        public int Id { get; set; }
        public string? definition { get; set; }
        //public List<object>? synonyms { get; set; }
        //public List<object>? antonyms { get; set; }
        public string? example { get; set; }

    }



    public class License
    {

        [Key]
        public int Id { get; set; }
        public string? name { get; set; }
        public string? url { get; set; }



    }

    public class Phonetic
    {

        [Key]
        public int Id { get; set; }

        public string? text { get; set; }
        public string? audio { get; set; }
        public string? sourceUrl { get; set; }

    }
    public class Meaning
    {


        [Key]
        public int Id { get; set; }
        public string? partOfSpeech { get; set; }
        public ICollection<Definition> definitions { get; set; }
        //public List<object>? synonyms { get; set; }
        //public List<object>? antonyms { get; set; }

    }
    public class dictionaryRoot
    {
        [Key]
        public int id { get; set; }
        public int BoxId { get; set; }

        public string? audioFile { get; set; }        
        public string word { get; set; }
        public List<Phonetic>? phonetics { get; set; }
        public List<Meaning>? meanings { get; set; }
        public License? license { get; set; }
        public List<string?>? sourceUrls { get; set; }
        public string? GTAnswer { get; set; }
    }
    public class dictionaryRootDto
    {
        [Key]
        public int id { get; set; }
        public int questionId { get; set; }
        public string? audioFile { get; set; }

        public string? word { get; set; }
        public string? phonetic { get; set; }
        public ICollection<Phonetic>? phonetics { get; set; }
        public List<Meaning> meanings { get; set; }
        public License? license { get; set; }
        //public List<string>? sourceUrls { get; set; }
        public string? Farsi { get; set; }
        public string? YourTranslate { get; set; }
        public string? YourExample { get; set; }
        public int? DayNo { get; set; }
        public string? GTAnswer { get; set; }
        public int userBoxid { get; set; }
       // public List<SimiliarWords>? similiarWords { get; set; }

    }
    public class SimiliarWords
    {
        public int Boxid { get; set; }

        public string word { get; set; }
        public string? officialTranslate { get; set; }

       
    }
    public class SearchedWord
    {
        public int? Boxday { get; set; }
        public int? Boxid { get; set; }
        public string word { get; set; }
        public string? officialTranslate { get; set; }
        public List<SimiliarWords>? similiarWords { get; set; }

    }
}
