# Date and Time Pickers for Blazor

**Date and Time Pickers** are fully-functional calendar and clock components for both **Blazor Server** and **Blazor Web Assembly** projects. *Please note: These are two separate components and not a one unified component. But I do plan to make one in the future by combining both of them.*

## **Date Picker**
![Date Picker for Blazor](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-blue.jpg "Date Picker for Blazor")

The Date Picker is a calendar control that allows you to pick a date. The chosen date is stored in a **Nullable DateTime** variable in C#. The basic usage of the control is:

Usage:

```html
<DatePicker @bind-SelectedDate="YourDateProperty" />
```

**Please note:** For now, the component has a limitation that it works best only in one case i.e. when you have the component hidden initially and show/hide it via a button (as shown in the screenshots). Therefore, you will need to do the following as well to make it work.

```html
<DatePicker @ref="DatePicker"
	    @bind-SelectedDate="YourDateProperty"
	    IsVisible="false" />

<button class="btn" @onclick="Show_Calendar">Show Calendar</button>
```

```C#
@code {
	/// <summary>
    /// This is the date field which is used by our date picker via two-way binding
    /// </summary>
    protected DateTime? YourDateProperty { get; set; }

	/// <summary>
    /// The model for date picker
    /// </summary>
    protected DatePickerBase DatePicker { get; set; }

	/// <summary>
    /// Opens/Shows the date picker
    /// </summary>
    /// <param name="args">Mouse event arguments</param>
    protected void Show_Calendar(MouseEventArgs args)
    {
        if (DatePicker.IsVisible) DatePicker.Hide();
        else DatePicker.Show();
    }
}
```
This component accepts the following parameters:

### **@bind-SelectedDate**
This is the property in your code that you want the date to bind to. Since the binding here is a two-way binding, the initial value of your property gets passed to the picker and the date is selected accordingly. Your property is also updated automatically whenever you change the date in the picker.

### **Month and Year**
These two parameters specify the initial month and year you want the picker to initialize to. These parameters are required if the **DateTime** property for the **SelectedDate** binding is initially **NULL** but the picker has to be initialized to some month and year to begin with. If the **SelectedDate** binding is initially **NOT NULL**, these parameters are not required as the picker is initialized to the date of that binding.  If no parameter is provided, the picker is initialized to the current date.

Usage:

```html
<DatePicker @bind-SelectedDate="YourDateProperty"
	    Month="12"
	    Year="2001" />
```

### **Theme**
This parameter controls the look and feel of the picker. I have given support for six colored themes out of the box as shown below:

![Date Picker for Blazor in Blue](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-blue.jpg "Date Picker for Blazor in Blue") ![Date Picker for Blazor in Black](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-black.jpg "Date Picker for Blazor in Black") ![Date Picker for Blazor in Red](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-red.jpg "Date Picker for Blazor in Red") ![Date Picker for Blazor in Pink](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-pink.jpg "Date Picker for Blazor in Pink") ![Date Picker for Blazor in Green](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-green.jpg "Date Picker for Blazor in Green") ![Date Picker for Blazor in Antique White](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-date-picker-component-antiquewhite.jpg "Date Picker for Blazor in Antique White")

Usage:

```html
<DatePicker @bind-SelectedDate="YourDateProperty"
	    Theme="SumitMehta.BlazorComponents.Shared.Enums.Themes.Blue" />
```

By default, the theme is set to **Blue**. You can also have your own custom themes. Please check the section **Customizing Themes** below.

### **Width**
This parameter allows you to specify the width of the picker. By default, no width is specified and the component auto sizes. I recommend you use the default behavior for the best results but in case you want to change it for some special case, that option is there for you.

Usage:

```html
<DatePicker @bind-SelectedDate="YourDateProperty"
	    Width="900" />
```

### **Culture**
This parameter is used for globalization. The date picker supports all the cultures that are supported by the .NET Core Framework. You need to pass the ISO code of the culture via this parameter.

Usage:

```html
<DatePicker @bind-SelectedDate="YourDateProperty"
	    Culture="fr" /> <!-- French -->
```

### **IsVisible**
This parameter controls the visibility of the picker.

Usage:

```html
<DatePicker @bind-SelectedDate="YourDateProperty"
	    IsVisible="true" />
```

### **SelectedDateChanged**
This parameter is an event callback which is trigerred whenever the date is changed in the picker. The callback accepts a **Nullable DateTime** as a parameter, which is the newly chosen date. In normal circumstances, you may not need to use this event callback explicitly as it's automatically generated by the **@bind-SelectedDate** binding. But if you don't want to use the binding and have some logic or functionality that requires some custom operation to be performed whenever the date is changed in the picker, you can use this event callback for the same.

Usage:

```html
<DatePicker SelectedDateChanged="(newDate) => ...." />
```

## **Time Picker**
![Time Picker for Blazor](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-blue.jpg "Time Picker for Blazor")

The Time Picker is an analog clock control that allows you to pick a time by rotating the clock hands for hour, minutes and seconds. The chosen time is stored in a **Nullable TimeSpan** variable in C#. The basic usage of the control is:

Usage:

```html
<TimePicker @bind-SelectedTime="YourTimeProperty" IsEnabled="@YourTimeProperty.HasValue" />
```
**Please note:** For now, the component has a limitation that it works best only in one case i.e. when you have the component hidden initially and show/hide it via a button (as shown in the screenshots). Therefore, you will need to do the following as well to make it work.

```html
<TimePicker @ref="TimePicker"
	    @bind-SelectedTime="YourTimeProperty"
	    IsEnabled="@YourTimeProperty.HasValue"
	    IsVisible="false" />

<button class="btn" @onclick="Show_Clock">Show Clock</button>
```

```C#
@code {
	/// <summary>
    /// This is the time field which is used by our time picker via two-way binding
    /// </summary>
    protected TimeSpan? YourTimeProperty { get; set; }

	/// <summary>
    /// The model for time picker
    /// </summary>
    protected TimePickerBase TimePicker { get; set; }

	/// <summary>
    /// Opens/Shows the time picker
    /// </summary>
    /// <param name="args">Mouse event arguments</param>
    protected void Show_Clock(MouseEventArgs args)
    {
        if (TimePicker.IsVisible) TimePicker.Hide();
        else TimePicker.Show();
    }
}
```

This component accepts the following parameters:

### **@bind-SelectedTime**
This is the property in your code that you want the time to bind to. Since the binding here is a two-way binding, the initial value of your property gets passed to the picker and the time is selected accordingly. Your property is also updated automatically whenever you change the time in the picker.

### **IsEnabled**
This parameter shows/hides the clock hands. This is usually controlled internally by the picker but you can provide the initial value via this parameter. Internally, the clock hands are shown on the click of the circular button at the center. Clicking it again hides them, hence the button working as a toggle button. It is implemented this way so that you can have a NULLABLE time. Therefore, if your user doesn't want to provide a value for an optional time field, he/she can simply click this button, which will hide all the clock hands and the **TimeSpan** property in your code will be set to NULL.

### **Hour, Minute and Second**
These three parameters specify the initial hour, minute and second you want the picker to initialize to. These parameters are required if the **TimeSpan** property for the **SelectedTime** binding is initially **NULL** but the picker has to be initialized to some time value to begin with. If the **SelectedTime** binding is initially **NOT NULL**, these parameters are not required as the picker is initialized to the time of that binding. If no parameter is provided, the picker is initialized to the current time.

Usage:

```html
<TimePicker @bind-SelectedTime="YourTimeProperty" 
            IsEnabled="@YourTimeProperty.HasValue"
            Hour="10"
            Minute="10"
            Second="10" />
```

### **Theme**
This parameter controls the look and feel of the picker. I have given support for six colored themes out of the box as shown below:

![Time Picker for Blazor in Blue](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-blue.jpg "Time Picker for Blazor in Blue") ![Time Picker for Blazor in Black](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-black.jpg "Time Picker for Blazor in Black") ![Time Picker for Blazor in Red](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-red.jpg "Time Picker for Blazor in Red") ![Time Picker for Blazor in Pink](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-pink.jpg "Time Picker for Blazor in Pink") ![Time Picker for Blazor in Green](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-green.jpg "Time Picker for Blazor in Green") ![Time Picker for Blazor in Antique White](https://sumitmehta.net/wp-content/uploads/2021/01/blazor-time-picker-component-antiquewhite.jpg "Time Picker for Blazor in Antique White")

Usage:

```html
<TimePicker @bind-SelectedTime="YourTimeProperty" 
            IsEnabled="@YourTimeProperty.HasValue"
            Theme="SumitMehta.BlazorComponents.Shared.Enums.Themes.Blue" />
```

By default, the theme is set to **Blue**. You can also have your own custom themes. Please check the section **Customizing Themes** below. 

### **Culture**
This parameter is used for globalization. The time picker supports all the cultures that are supported by the .NET Core Framework. You need to pass the ISO code of the culture via this parameter.

Usage:

```html
<TimePicker @bind-SelectedTime="YourTimeProperty" 
            IsEnabled="@YourTimeProperty.HasValue"
            Culture="fr" /> <!-- French -->
```

### **IsVisible**
This parameter controls the visibility of the picker.

Usage:

```html
<TimePicker @bind-SelectedTime="YourTimeProperty" 
            IsEnabled="@YourTimeProperty.HasValue"
            IsVisible="true" />
```

### **SelectedTimeChanged**
This parameter is an event callback which is trigerred whenever the time is changed in the picker. The callback accepts a **Nullable TimeSpan** as a parameter, which is the newly chosen time. In normal circumstances, you may not need to use this event callback explicitly as it's automatically generated by the **@bind-SelectedTime** binding. But if you don't want to use the binding and have some logic or functionality that requires some custom operation to be performed whenever the time is changed in the picker, you can use this event callback for the same.

Usage:

```html
<TimePicker SelectedTimeChanged="(newTime) => ...." />
```

## Customizing Themes
While we give you six themes for both the **Date and Time Pickers**, you can always have your own custom themes. Below is the CSS code that you need to customize in order to have your own custom themes. The code samples are taken here for the **Black** theme. You will need to add the new color name to the enum **SumitMehta.BlazorComponents.Shared.Enums.Themes** and use that newly added color name in the CSS below.

### **Date Picker**

```css
/* Black */
.date-picker.black tr td.right, /* The colored right hand side of the picker */
.date-picker.black tr td.left, /* The colored left hand side of the picker */
.date-picker.black .calendar tr td.date.selected { /* Style for the selected date */
    background-color: black;
}

.date-picker.black .calendar tr td.date.selected { /* Style for the selected date */
    color: white;
}

.date-picker.black .calendar .selected-date-day { /* Style for the selected date's day (displayed in the calendar header on the top left) */
    color: gray;
}

.date-picker.black .calendar tr td.month.active { /* Style for the selected (active) month */
    color: black;
}

.date-picker.black .calendar tr td.month:hover { /* Style for months when we hover over each of them */
    color: gray !important;
}

.date-picker.black .calendar tr td .year-switcher .left-arrow:hover { /* Style for the left button of the year switcher when we hover over it */
    border-right: 20px solid black;
}

.date-picker.black .calendar tr td .year-switcher .right-arrow:hover { /* Style for the right button of the year switcher when we hover over it */
    border-left: 20px solid black;
}
```
### **Time Picker**
```css
 /* Black */
.time-picker.black { /* Background of the clock */
    background-color: white;
    border: 12px solid black;
}

    .time-picker.black ul li { /* Common style for clock numbers around the clock */
        color: gray;
    }

    .time-picker.black .secondsHand { /* Seconds hand */
        background-color: gray;
    }
```

## Accessing component models from C# code-behind
You can also access the components from C# code. This is useful and may be needed for some use cases like showing/hiding the pickers on the click of a button. This is demonstrated in the demo projects included in this repository but just to brief, you just need to add `@ref` attrbute on the component and hook it with the corresponding property in your C# code. The type of the property will be the type of the model of the component as mentioned below.

For **Date Picker**: `SumitMehta.BlazorServer.Components.DatePickerBase/SumitMehta.BlazorWebAssembly.Components.DatePickerBase`

and 

For **Time Picker**: `SumitMehta.BlazorServer.Components.TimePickerBase/SumitMehta.BlazorWebAssembly.Components.TimePickerBase`

## Known Issues
While the **Date Picker** seems to be working just fine, the **Time Picker** may need some enhancements. The rotation of the clock arms has some lag, especially when used in **Blazor Server** projects. This is quite understandable because **signalR** notifications aren't as quick as the drag operation on the client side is, and frequent re-drawing of that particular part of DOM causes that lag. However, there is also some lag when the component is used in **Blazor WebAssembly** projects. The lag is not as much as it's for **Blazor Server** projects but it still exists. Due to lack of time, I couldn't look into it much but I will have a look in the future if I can hook a native javascript code there and tie it up somehow with the C# code. I am also looking forward to some vital contributions from the other contributors to make this component better for the community.

The components are also not yet optimized for mobile and small devices. This is on priority and will be sorted out soon.

## Credits
The UI has not been designed by me entirely (I specialize more in backend programming). I have taken bare bone static code and then done modifications/enhancements over it. Below are the reference projects that I have used. I want to thank the project owners of these projects as their UI contribution helped me speed my work a bit.

[JS+CSS Clock with Sound by Ahmad Emran](https://codepen.io/ahmadbassamemran/pen/WdKQyx)

[Daily CSS Images - Calendar by Alex Johnson](https://codepen.io/IAmAlexJohnson/pen/ybgwQG)

## About The Code
This repository has one solution that can be opened in Visual Studio 2019. The solution has 5 projects:

1. **SumitMehta.BlazorComponents.Shared** - This project has the shared code which is common to both the **Blazor Server** and **Blazor WebAssembly** projects.

2. **SumitMehta.BlazorServer.Components** - This project has the **Date and Time Pickers** for **Blazor Server** projects.

3. **SumitMehta.BlazorWebAssembly.Components** - This project has the **Date and Time Pickers** for **Blazor WebAssembly** projects.

4. **SumitMehta.BlazorServer.Components.Test** - This is a **Blazor Server** project for showing the **Date and Time Pickers** in action.

5. **SumitMehta.BlazorWebAssembly.Components.Test** - This is a **Blazor WebAssembly** project for showing the **Date and Time Pickers** in action.

## Installation
You can also get these components via NuGet. Use the following commands in the package manager to install the latest version of the respective package.

**Blazor Server**: `Install-Package SumitMehta.BlazorServer.Components`

**Blazor WebAssembly**: `Install-Package SumitMehta.BlazorWebAssembly.Components`

## About Me
My name is Sumit Mehta and I have been into .NET development for quite a few years now. Blazor is relatively a new SPA stack and I have developed these components just after going through a crash course on YouTube and getting a taste of Blazor. I am not yet a full-time Blazor Developer by any means however, .NET background really helped me get started with Blazor quickly.

Please feel free to [check my website](https://sumitmehta.net "Contact Sumit Mehta") and get in touch with me.

## License

The MIT License (MIT)

Copyright (c) 2008-2021 Sumit Mehta

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.