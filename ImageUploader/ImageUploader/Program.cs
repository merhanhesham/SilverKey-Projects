using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;



var app = WebApplication.Create();
app.MapGet("/", (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    string htmlCode = @"<!DOCTYPE html>
<html>

<head>
<style>
body{
background-color:darkgrey;
padding: 100px;
}
.title, .UploadedImg{
width: 100%;
background-color: white;
padding: 14px 20px;
margin: 8px 0;
border: none;
border-radius: 4px;
cursor: pointer;
margin-bottom: 30px;
}

.submitbtn{

background-color: blue;
border: none;
color: white;
padding: 10px 25px;
text-align: center;
text-decoration: none;
display: inline-block;
font-size: 16px;
border-radius: 10px;
cursor: pointer;
}
h2{
margin-bottom: 40px;
}
.userErrorMsg{
text-align: center;
margin-bottom:20px;
color: red;
}


</style>

</head>

<body>
<div>
<h2> Image Uploader </h2>
<form name=""imageForm""  method=""post"" enctype=""multipart/form-data"">
<label for= ""imageTitle"">
<h5> Image Title </h5>
</label>
<input class= ""title"" type=""text"" placeholder=""Image Title"" id=""imageTitle"" name=""imagetitle"" />

<label for=""imagefile"">
<h5>Upload Image</h5>
</label>
<input class= ""UploadedImg"" type=""file"" id=""imagefile"" name=""imagefile""  />
<div class= ""userErrorMsg""></div>
<input class= ""submitbtn"" onclick=AddImage() type=""submit"" value = ""upload""></input>
</form>
</div>

<script>
var TitleInput=document.querySelector('.title');
var ImageInput = document.querySelector('.UploadedImg');
var userErrorMsg = document.querySelector('.userErrorMsg');

function AddImage()
{
Validate();
}

function Validate()
{
var ImageUploaderObj ={
title: TitleInput.value,
UploadedImg: ImageInput.value
}
   
var extension = ImageUploaderObj.UploadedImg.split('.').pop().toLowerCase();

if (ImageUploaderObj.title.length == 0 || ImageUploaderObj.UploadedImg.length == 0)
{
userErrorMsg.innerHTML = ""All Inputs are required"";
}
else if (extension != ""png"" && extension != ""jpeg"" && extension != ""gif""&&extension!=""jpg""){
userErrorMsg.innerHTML = ""Please upload an image of these types jpeg, png or gif"";
}
else
{
userErrorMsg.style.color = 'green';
userErrorMsg.innerHTML = 'success';

}
}

</script>
</body>

</html>";
    return context.Response.WriteAsync(htmlCode);

});
app.MapPost("/", async (HttpContext context) =>
{
    IFormCollection form = await context.Request.ReadFormAsync();
    string? title = form["imagetitle"];
    var file = form.Files.GetFile("imagefile");

    if (string.IsNullOrEmpty(title))
    {
        return Results.BadRequest("please fill in the title input!");
    }
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("please select an image file!");
    }
    var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    var fileExtension = Path.GetExtension(file.FileName).ToLower();
    if (!allowedExtentions.Contains(fileExtension))
    {
        return Results.BadRequest("Only jpg, png, and gif are supported.");

    }
    var filePath = Path.GetExtension(file.FileName);
    var imageID = Guid.NewGuid().ToString();
    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "mypicture", $"{imageID}{filePath}");
    using (var fileStream = new FileStream(imagePath, FileMode.Create))
    {
        await file.CopyToAsync(fileStream);
    }

    var imageDetails = new
    {
        Title = title,
        Id = imageID,
        imgPath = imagePath,
    };


    var json = JsonSerializer.Serialize(imageDetails);
    var jsonPath = Path.Combine("imageData.json");
    File.AppendAllText(jsonPath, $"{json}");

    var imageURL = $"/mypicture/{imageID}";
    return Results.Redirect(imageURL);
});

app.MapGet("/mypicture/{id}", async (string id, HttpContext context) =>
{
    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "imageData.json");
    var allLines = await File.ReadAllLinesAsync(jsonPath);
    var allImages = new List<Image>();

    foreach (var line in allLines)
    {
        var myImage = JsonSerializer.Deserialize<Image>(line);
        allImages.Add(myImage);
    }

    if (allImages.Count == 0)
    {
        return Results.NotFound("No images exists.");
    }

    var image = allImages.FirstOrDefault(i => i.Id == id);
    if (image == null)
    {
        return Results.NotFound("Image Not Found.");
    }
    byte[] imageBytes = await File.ReadAllBytesAsync(image.imgPath);
    string imgbase64 = Convert.ToBase64String(imageBytes);

    context.Response.ContentType = "text/html";
    var htmlCode = $@"
     <!DOCTYPE html>
<html>

<head>
    <title>Image Uploader Form</title>
    <meta name=""viewport"" content=""width=device-width"" , initial-scale=""1.0"">
    <style>
        body{{
    
    background-color:darkgrey;
    padding: 100px;
    }}

    img
    {{width: 50%;
    margin-bottom: 10px;
    border-radius: 10px;
    }}
    .imgdisplay
    {{text-align: center;
    }}
    </style>

</head>

<body>
    <div class=""imgdisplay"">
    <h2>Here's the picture</h2>
    <div>
    <img src=""data:image/png;base64,{imgbase64}"" alt=""{image.Title}"" />
    <h4>{image.Title}</h4>
    </div>

    </div>
  
</body>
</html>
";

    return Results.Text(htmlCode, "text/html");

});


app.Run();
public class Image
{
    public string Title { get; set; }
    public string Id { get; set; }
    public string imgPath { get; set; }
    public string FileName { get; set; }
}